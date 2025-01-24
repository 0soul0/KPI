$(function () {
    bindEvent();
});

function bindEvent() {
    $(".add-row-item").on("click", function () {
        loadAjax(
            'POST',
            '/Form/AddItemRow',
            {
                "Type": $(this).data("type"),
                "RowCount": $("tr").length
            },
            function (response) {
                $("table tbody").append(response); // 動態添加行
            },
            function (error) {   // 自定义错误处理
                console.log("error:" + error)
            },
            false
        )
    });

    $("#selectable").on("mousedown", function (e) {
        e.metaKey = true;
    }).selectable();

    $("#kpiModal").on('show.bs.modal', function (event) {
        var id = $(event.relatedTarget).data('id');
        var kpiList = $(`#${id} .kpi span`);

        $(".ui-state-default").removeClass("ui-selected")
        kpiList.each(function () {
            var spanId = $(this).attr('data-id')
            console.log(spanId)
            $(".ui-state-default").siblings(`[data-id="${spanId}"]`).addClass("ui-selected")
        })
        $("#kpiId").text(`#${id}`)
        console.log(id)

    })

    $("#kpiConfirm").on('click', function () {

        let kpiList = [];
        $(".ui-selected").each(function () {
            kpiList.push(`<span data-id=${$(this).attr('data-id')}>${$(this).text().trim()}</span>`);
        });
        kpiList = kpiList.join(", ");
        var tr = $("#kpiId").text()
        $(`${tr} .kpi`).html(kpiList)
        syncHeights(tr)
    })

    //$("#sortable").sortable({
    //    handle: ".index",
    //    placeholder: "portlet-placeholder ui-corner-all"
    //});
    //$("#sortable").disableSelection();
    $("#save").on('click', function () {
        function isRowEmpty(row) {
            // 遍歷所有需要檢查的列
            for (let i = 0; i <= 4; i++) {
                if (row.eq(i).text().trim() !== "") {
                    return false; // 有內容則不為空
                }
            }
            return true; // 全部為空
        }

        function isDataEmpty(head,content) {

            if (formHead.Year == "" || formHead.Year == 0) {
                alert($('label[for="FormHead_Year"]').text() + "不得為空")
                return true
            }
            if (formHead.Name == "") {
                alert($('label[for="FormHead_Name"]').text() + "不得為空")
                return true
            }
            if (formContentList.length == 0) {
                alert("資料不得為空")
                return true
            }
        }

        let href = "/" + $(location).attr('href').split("/").slice(3, 5).join("/")
        log(href)
        let formHead = {
            FormHeadId: $("#FormHead_FormHeadId").val().trim(),
            Name: $("#FormHead_Name").val().trim(),
            Year: $("#FormHead_Year").val().trim(),
        }
        let formContentList = []
    
        $("tr").each(function (index) {
            if (index == 0) return true

            var row = $(this).find("td > .text")
            if ($(this).data("type") == "title") {
                formContentList.push(
                    {
                        FormContentId: "",
                        FormHeadId: "",
                        RowIndex: index,
                        CategoryName: row.eq(0).text(),
                    }
                )

            } else {
                //if (isRowEmpty(row)) return true
                formContentList.push(
                    {
                        FormContentId: "",
                        FormHeadId: "",
                        RowIndex: index,
                        CheckIndicators: row.eq(0).text().trimEnd(),
                        CheckStandards: row.eq(1).text().trimEnd(),
                        AttachedInfo: row.eq(2).text().trimEnd(),
                        Assessment: row.eq(3).text().trimEnd(),
                        KpiLinkage: row.eq(4).text().trimEnd(),
                    }
                )

            }
           
        })

        if (isDataEmpty(formHead, formContentList)) {
            return true
        }

        loadAjax(
            'POST',
            href,
            {
                "FormHead": formHead,
                "FormContentList": formContentList
            },
            function (response) {   // 自定义错误处理
                if (response.code == 200) {
                    window.location = window.location.origin +"/Form/Index"
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