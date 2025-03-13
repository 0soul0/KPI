$(function () {
    bindEvent();
});


function bindEvent() {

    let selectedUnits = []
    $("#create").on('click', function () {

        selectedUnits = $(".ui-selected").map((_, el) => ({
            UnitId: String($(el).data('id')),
            Name: $(el).text()
        })).get();
        if (selectedUnits.length === 0) return alert("請選擇單位或是中心")
        let selectedFromId = $("select[name='SelectedFromId']").val();
        if (selectedFromId == "") return alert("請選擇表單")
        let href = "/" + $(location).attr('href').split("/").slice(3, 4).join("/") + "/CreateFrom"
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
                    log(response.data)
                    var model = $.parseJSON(response.data)
                    createTable($.parseJSON(model.Data), "spreadsheet")
                    $("#save").removeClass("d-none")
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
        let href = "/" + $(location).attr('href').split("/").slice(3, 5).join("/")
        loadAjax(
            'POST',
            href,
            {
                "EvaluationId": $("#EvaluationId").text().trim(),
                "Data": JSON.stringify(data),
                "FromName": handsontable.getDataAtCell(0, 1),
                "Year": handsontable.getDataAtCell(0, 0),
                "Units": units
                //"EvaluationId": $("#FormId").text().trim()
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    window.location = window.location.origin + "/Evaluation/Index"
                }
                log(response)
            },
            function (error) {   // 自定义错误处理
                log(error)
            },
            false
        )
    })
}

function getFormTable() {
    createTable($.parseJSON($("#Data").text()), 'spreadsheet');
}