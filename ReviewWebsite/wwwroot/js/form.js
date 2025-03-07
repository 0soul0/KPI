$(function () {
    bindEvent();
    createTable($.parseJSON($("#Data").text()), 'spreadsheet');
});

function bindEvent() {
    //$(".add-row-item").on("click", function () {
    //    loadAjax(
    //        'POST',
    //        '/Form/AddItemRow',
    //        {
    //            "Type": $(this).data("type"),
    //            "RowCount": $("tr").length
    //        },
    //        function (response) {
    //            $("table tbody").append(response); // 動態添加行
    //        },
    //        function (error) {   // 自定义错误处理
    //            console.log("error:" + error)
    //        },
    //        false
    //    )
    //});

    //$("#kpiModal").on('show.bs.modal', function (event) {
    //    var id = $(event.relatedTarget).data('id');
    //    var kpiList = $(`#${id} .kpi span`);

    //    $(".ui-state-default").removeClass("ui-selected")
    //    kpiList.each(function () {
    //        var spanId = $(this).attr('data-id')
    //        console.log(spanId)
    //        $(".ui-state-default").siblings(`[data-id="${spanId}"]`).addClass("ui-selected")
    //    })
    //    $("#kpiId").text(`#${id}`)
    //    console.log(id)

    //})

    //$("#kpiConfirm").on('click', function () {

    //    let kpiList = [];
    //    $(".ui-selected").each(function () {
    //        kpiList.push(`<span data-id=${$(this).attr('data-id')}>${$(this).text().trim()}</span>`);
    //    });
    //    kpiList = kpiList.join(", ");
    //    var tr = $("#kpiId").text()
    //    $(`${tr} .kpi`).html(kpiList)
    //    syncHeights(tr)
    //})

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