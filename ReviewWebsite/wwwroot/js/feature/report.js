$(function () {
    bindEvent();
});
var page = 1
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
                    $("#output").show()
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
