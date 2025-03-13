$(function () {
    bindEvent();
});
let page = 1
function bindEvent() {
    $("#more").on('click', function () {
        page++
        let href = "/" + $(location).attr('href').split("/").slice(3, 4).join("/") + "/More?page=" + page
        loadAjax(
            'Get',
            href,
            {},
            function (response) {   // 自定义错误处理
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
    $("#create").on('click', function () {

        selectedEvaluations = $(".ui-selected").map((_, el) => ({
            EvaluationId: String($(el).data('id')),
        })).get();
        if (selectedEvaluations.length === 0) return alert("請選擇單位或是中心")
        let href = "/" + $(location).attr('href').split("/").slice(3, 4).join("/") + "/CreateReport"
        log(href)

        loadAjax(
            'POST',
            href,
            {
                "SelectedEvaluations": selectedEvaluations
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    createTable($.parseJSON(response.data), "spreadsheet")
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
