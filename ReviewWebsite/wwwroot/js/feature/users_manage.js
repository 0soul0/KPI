

$(function () {
    bindEvent();
    dealUrlFrom();
    createUserOrUnitTable();
});


function createUserOrUnitTable() {
    if ($("#userTable").is("*")) {
        createPrettyTable('#userTable');
    } else if ($("#unitTable").is("*")) {
        createPrettyTable('#unitTable');
    }

}

function dealUrlFrom() {
    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('from') == "unit") {
        $("#unit-btn").trigger('click');
    }
}

function bindEvent() {
    $("#userInfoModal").on('show.bs.modal', function (event) {
        var userId = $(event.relatedTarget).data('id');
        var userInfo = $("#" + userId + " td");

        $("#UserId").val(userId);
        $("#Name").val(userInfo.get(1).textContent.trim());
        $("#Email").val(userInfo.get(2).textContent.trim());
        $("#Birthday").val(userInfo.get(3).textContent.trim());
        $("#Telephone").val(userInfo.get(4).textContent.trim());
        $("#AccessRight").val(userInfo.get(5).dataset.accessNumber);
    });

    $("#submitUser").on('click', function () {
        var user = {
            "UserId": $("#UserId").val(),
            "AccessRight": $("#AccessRight").val(),
        }
        loadAjax(
            'POST',
            getCurrentHref() + "/EditUser" +,
            {
                "User": user
            },
            function (response) {   // 自定义错误处理
                // 插入內容
                history.pushState(null, '', href);
                $('#body-content').html(response);
                dynamicLoadScriptsAndCss()
            },
            function (error) {   // 自定义错误处理
                console.log("error:" + error)
            }
        )
    })

    $("#submitUnit").on('click', function () {

        var unit = {
            "Name": $("#Name").val()
        }

        loadAjax(
            'POST',
            getCurrentHref()+"/CreateUnit"+,
            {
                "Unit": unit
            },
            function (response) {   // 自定义错误处理
                // 插入內容
                history.pushState(null, '', href);
                $('#body-content').html(response);
                dynamicLoadScriptsAndCss()
            },
            function (error) {   // 自定义错误处理
                console.log("error:" + error)
            }
        )
    })

}
