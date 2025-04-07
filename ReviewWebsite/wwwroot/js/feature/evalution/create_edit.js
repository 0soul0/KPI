$(function () {
    bindEvent();
    getOrCheckEdit();
});
function bindEvent() {

    let selectedUnits = []
    $("#createTable").on('click', function () {

        selectedUnits = $(".selected_item").map((_, el) => ({
            UnitId: String($(el).data('id')),
            Name: $(el).text()
        })).get();
        if (selectedUnits.length === 0) return alert("請選擇單位或是中心")
        let selectedFromId = $("select[name='SelectedFromId']").val();
        if (selectedFromId == "") return alert("請選擇表單")
        let href = getCurrentHref().replace("Create", "CreateFrom")
        log(href)

        loadAjax(
            'POST',
            href,
            {
                "SelectedFromId": selectedFromId,
                "SelectedUnits": selectedUnits
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    ss = response
                    var model = $.parseJSON(response.data)
                    var unitsLen = model.Units.length + 1
                    createEvalutionTable($.parseJSON(model.Data), unitsLen)
                }
                log(response)
            },
            function (error) {   // 自定义错误处理
                log(error)
            },
            false
        )
    })
    $("#save").on('click', function () {

        var data = handsontable.getData()
        var units = selectedUnits.map(unit => unit.Name).join(",")
        let href = getCurrentHref()
        loadAjax(
            'POST',
            href,
            {
                "EvaluationId": $("#EvaluationId").text().trim(),
                "Data": JSON.stringify(data),
                "FromName": handsontable.getDataAtCell(0, 1),
                "Year": handsontable.getDataAtCell(0, 0),
                "Units": units,
                "UpdateTime": $("#UpdateTime").text().trim()
                //"EvaluationId": $("#FormId").text().trim()
            },
            function (response) {
                if (response.code == 200) {
                    window.location = window.location.origin + "/Evaluation/Index"
                }
                log(response)
            },
            function (error) {   // 自定义错误处理
                if (error.code == 501) {
                    alert(error.message)
                }
                log(error)
            },
            false
        )
    })
}

function getOrCheckEdit() {
    if ($("#Data").is("*")) {
        var data = $.parseJSON($("#Data").text())
        createEvalutionTable(data, data[0].length - 6)
    }
}

function createEvalutionTable(data, unitsLen, callback) {
    $("#excelTable").show()
    createExcelTable(data, 'spreadsheet', afterRender = function (changes) {

    }, onCreateDone = function (hot) {
        for (let row = 0; row < hot.countRows(); row++) {
            var colLen = hot.countCols()
            for (let col = 0; col < colLen - unitsLen; col++) {
                hot.setCellMeta(row, col, 'readOnly', true);
            }
            for (let col = colLen - unitsLen; col < colLen; col++) {
               
                //限制KPI連動
                 const kpi = hot.getDataAtCell(row, 5);
                if (kpi !== null && kpi !== '' && kpi.split(",").indexOf(cookieUser.Unit.Name)==-1) {
                    hot.setCellMeta(row, col, 'readOnly', true);
                /*    setRowClass(hot, row, 'bg-secondary bg-opacity-25')*/
                    hot.setCellMeta(row, col, 'className', 'bg-secondary bg-opacity-25');
                    continue
                }
                //設定下拉選單
                const score = hot.getDataAtCell(row, 2);
                if (isNaN(score) || score == null || score == '') {
                    hot.setCellMeta(row, col, 'readOnly', true);
                } else {
                    const source = Array.from({ length: score }, (_, i) => i + 1);
                    hot.setCellMeta(row, col, 'source', source);
                }
           
                
            }
         
        }
    });
   
}


function setRowClass(hot,rowIndex, className) {
    const colCount = hot.countCols();
    for (let colIndex = 0; colIndex < colCount; colIndex++) {
        hot.setCellMeta(rowIndex, colIndex, 'className', className);
    }
}
