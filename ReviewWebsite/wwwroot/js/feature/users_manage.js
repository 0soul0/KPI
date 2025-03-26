

$(function () {
    log("Loading users_manage.js success")
    bindEvent();
    //dealUrlFrom();
    createUserOrUnitTable();
});

function createUserOrUnitTable() {
    if ($("#userTable").is("*")) {
        createPrettyTable('#userTable');
    } else if ($("#unitTable").is("*")) {
        createPrettyTable('#unitTable');
    }

}

//function dealUrlFrom() {
//    var urlParams = new URLSearchParams(window.location.search);
//    if (urlParams.get('from') == "unit") {
//        $("#unit-btn").trigger('click');
//    }
//}

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
        loadAjax(
            'POST',
            getCurrentHref() + "/EditUser" ,
            {
                "UserId": $("#UserId").val(),
                "AccessRight": $("#AccessRight").val(),
            },
            function (response) {   // 自定义错误处理
                $('#userInfoModal').modal('hide')
                location.reload()
            },
            function (error) {   // 自定义错误处理
                console.log("error:" + error)
            }
        )
    })
    $("#submitUnit").on('click', function () {

        loadAjax(
            'POST',
            getCurrentHref()+"/CreateUnit",
            {   
                "UnitId":"",
                "Name": $("#Name").val()
            },
            function (response) {   // 自定义错误处理
                $('#addUnitModal').modal('hide')
                location.reload()
            },
            function (error) {   // 自定义错误处理
                console.log("error:" + error)
            }
        )
    })
}
