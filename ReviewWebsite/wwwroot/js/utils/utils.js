
//新增table
function createPrettyTable(id) {
    let table = new DataTable(id, {
        "columnDefs": [
            {
                "targets": "_all", // 針對所有列
                "className": "text-start" // 添加 CSS 類名
            }
        ],
        language: {
            search: "🔍 搜尋：",
            lengthMenu: "顯示 _MENU_ 筆資料",
            info: "顯示 _START_ 到 _END_ 筆，共 _TOTAL_ 筆",
            paginate: {
                first: "首頁",
                last: "最後",
                next: "下一頁",
                previous: "上一頁"
            },
            emptyTable: "沒有資料",
            infoEmpty: "沒有符合條件的資料",
            loadingRecords: "載入中...",
            zeroRecords: "找不到符合條件的結果"
        }
    });
}

//異步請求
function loadAjax(type, href, data, onSuccess, onError, changeUrl = true) {
    // 确保 `data` 存在并添加 token
    $.ajax({
        type: type,
        url: href,
        contentType: 'application/json',
        data: JSON.stringify({
            ...data, // 使用传入的 data
            //__RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value // 自动添加 token
        }), // 合并后的数据
        //data: requestData,
        success: function (response) {

            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }
        },
        error: function (error) {
            console.log("error:" + error)
            if (typeof onError === 'function') {
                onError(error);
            }
        }
    });
}

//動態載入js css
function dynamicLoadScriptsAndCss() {
    // 動態加載 JS 和 CSS
    $("meta[name='dynamic-styles']").each(function (index, element) {
        var style = $(element).data("href")
        if (style) {
            $('<link>')
                .attr('rel', 'stylesheet')
                .attr('href', style)
                .appendTo('head');
        }
    });

    $("meta[name='dynamic-scripts']").each(function (index, element) {
        var script = $(element).data("src")
        if (script) {
            $.getScript(script);
        }
    });
}

//log
function log(msg) {
    if (false) return false
    console.log(msg)
}

//綁定選項事件
function createSelectItem(id) {
    //selectable
    $("#" + id).on("mousedown", function (e) {
        e.metaKey = true;
    }).selectable();
}

function getCurrentHref() {

    var fullUrl = $(location).attr('href');  // 獲取完整 URL，例如 https://localhost:7045/UserManagement?from=user
    return new URL(fullUrl).origin + new URL(fullUrl).pathname;
}