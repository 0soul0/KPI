$(function () {
    bindCompentEvent();
    bindEvent();
});
var page = 1
var hot=null
function bindEvent() {
    $("#more").on('click', function () {
        page++
        let href = "/" + $(location).attr('href').split("/").slice(3, 4).join("/") + "/More?page=" + page
        log("page" + page)
        loadAjax(
            'GET',
            href,
            {},
            function (response) {   // 自定义错误处理
                log("response" + response)
                var list = $.parseJSON(response.data)
                var isLastPage = $.parseJSON(response.extraData)
                if (list.length > 0) {
                    var html = list.map(element =>
                        `<li class="ui-state-default px-3 py-1" data-id="${element.EvaluationId}">
                         ${element.Year}-${element.FromName}-${element.Units}
                        </li>`
                    ).join('');

                    $("#selectable").append(html);
                }

                if (isLastPage) {
                    $("#more").hide()
                }
            },
            function (error) {   // 自定义错误处理
                log(error)
            },
            false
        )
    })
    $("#createTable").on('click', function () {

        selectedEvaluations = $(".selected_item").map((_, el) => ({
            EvaluationId: String($(el).data('id')),
        })).get();
        if (selectedEvaluations.length === 0) return alert("評鑑表單名")
        let href = getCurrentHref() + "/CreateReport"
        log(href)

        loadAjax(
            'POST',
            href,
            {
                "SelectedEvaluations": selectedEvaluations
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    createEvalutionTable($.parseJSON(response.data), "spreadsheet")
                }
                log(response)
            },
            function (error) {   // 自定义错误处理
                log(error)
            },
            false
        )
    })
    $("#exportExcel").on('click', function () {
        exportExcel()
    })
}


function createEvalutionTable(data) {
    hot = createExcelTable(data, 'spreadsheet', afterRender = function (changes) {

    }, onCreateDone = function (hot) {
        for (let row = 0; row < hot.countRows(); row++) {
            for (let col = 0; col < hot.countCols(); col++) {
                hot.setCellMeta(row, col, 'readOnly', true);
            }
        }
        const mergePlugin = hot.getPlugin("mergeCells");
        mergePlugin.merge(0, 1, 0, 2);
    });
    $("#excelTable").show()
}

function exportExcel() {
    const exportPlugin = hot.getPlugin('exportFile');
    var filename = hot.getDataAtCell(0, 0)+"-"+hot.getDataAtCell(0, 1)
    exportPlugin.downloadFile('csv', {
        bom: true,
        columnDelimiter: ',',
        columnHeaders: false,
        exportHiddenColumns: true,
        exportHiddenRows: true,
        fileExtension: 'csv',
        filename: filename,
        mimeType: 'text/csv',
        rowDelimiter: '\r\n',
        rowHeaders: true,
    });
}