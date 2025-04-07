var hotTable = null;
let itemTxtToViewMap = {};
let hotTableData = null;
//let itemTxtToId = {};
$(function () {
    cacheData();
    bindEvent();
    initExcelTable();
    createPrettyTable('#formTable');
});

function cacheData() {
    $(".selectable_item").each(function () {
        var key = $(this).data("id")
        var value = $(this).text()
        itemTxtToViewMap[value] = this
    });
    hotTableData = $.parseJSON($("#Data").text())
}

function initExcelTable() {
    var selectedCell = null;
    hotTable = createExcelTable(hotTableData, 'spreadsheet', afterRender = function (changes, source) {
        $(".select_kpi").attr("data-bs-toggle", "modal").attr("data-bs-target", "#selectUnitModal");
    }, onCreateDone = function (hot) {

        $("#selectUnitModal").on('show.bs.modal', function (event) {
            //selectedCell = $(event.relatedTarget);
            $(".selectable_item").removeClass("selected_item")
            selectedCell = hotTable.getSelected()
            var units = hotTable.getDataAtCell(selectedCell[0][0], selectedCell[0][1])
            if (units != null) {
                units.trim().split(",").forEach(function (value, index) {
                    $(itemTxtToViewMap[value]).addClass("selected_item")
                })
            }

        });
    });

    $("#confirmUnit").on('click', function () {
        if (!selectedCell) return
        var strUnits = $(".selected_item").map(function () {
            return $(this).text();
        }).get().join(",");
        //var maxItemsPerLine = 3; // 每行最多 3 個
        //var formattedText = strUnits.map((item, index) => {
        //    return (index > 0 && index % maxItemsPerLine === 0) ? "\n" + item : item;
        //}).join(",");
        hotTable.setDataAtCell(selectedCell[0][0], selectedCell[0][1], strUnits)
        $('#selectUnitModal').modal('hide');
    })
}

function bindEvent() {
    $("#save").on('click', function () {

        var data = handsontable.getData()

        let url = getCurrentHref()
        let href = url.substring(0, url.lastIndexOf('/'));
        loadAjax(
            'POST',
            href,
            {
                "Data": JSON.stringify(data),
                "Name": handsontable.getDataAtCell(0, 1),
                "Year": handsontable.getDataAtCell(0, 0),
                "FormId": $("#FormId").text().trim(),
                "UpdateTime": $("#UpdateTime").text().trim(),
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    window.location = window.location.origin + "/Form/Index"
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