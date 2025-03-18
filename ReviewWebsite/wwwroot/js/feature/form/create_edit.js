$(function () {
    bindEvent();
    createTable($.parseJSON($("#Data").text()), 'spreadsheet');
    createPrettyTable('#formTable');
});

function bindEvent() {
    $("#save").on('click', function () {
        
        var data = handsontable.getData()

        let href = "/" + $(location).attr('href').split("/").slice(3, 5).join("/")
        loadAjax(
            'POST',
            href,
            {
                "Data": JSON.stringify(data),
                "Name": handsontable.getDataAtCell(0, 1),
                "Year": handsontable.getDataAtCell(0, 0),
                "FormId": $("#FormId").text().trim()
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    window.location = window.location.origin + "/Form/Index"
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

function syncHeights(event) {
    let row = $(event)
    if (!$(event).is($("tr"))) {
        row = $(event).parents("tr")
    }
    const divs = row.find("td > div")
    let maxHeight = 0;
    divs.each(function () {
        const div = $(this);
        div.height("auto"); // 重設高度以正確測量內容高度
        maxHeight = Math.max(maxHeight, div.height());
    });
    divs.height(maxHeight);
}

function removeItemRow(event) {
    if ($(".index").length <= 1) {
        alert("至少需要一行")
        return
    }
    $(event).parents("tr").remove()
    //重新排序
    resetIndexSort()
}

function resetIndexSort() {
    for (var i = 0; i < $(".index").length; i++) {
        $(".index").get(i).textContent = i + 1;
    }
}