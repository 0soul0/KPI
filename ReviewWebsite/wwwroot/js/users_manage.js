

$(function () {
    bindEvent();
    dealUrlFrom();
});

function dealUrlFrom() {
    var urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get('from') == "unit") {
        $("#unit-btn").trigger('click');
    }
} 

function updateQueryParam(key, value) {
    var url = new URL(window.location.href); 
    url.searchParams.set(key, value); 
    window.history.pushState({}, '', url); 
}

function bindEvent() {
    $("#user-btn").on("click", function () {
        $("#user-list").show();
        $("#unit-list").hide();
        $("#add-unit").hide();
        $(this).addClass("active_tag");
        $("#unit-btn").removeClass("active_tag");
        updateQueryParam("from", "user");
    });
    $("#unit-btn").on("click", function () {
        $("#unit-list").show();
        $("#user-list").hide();
        $("#add-unit").show();
        $(this).addClass("active_tag");
        $("#user-btn").removeClass("active_tag");
        updateQueryParam("from", "unit");
    });
    $("#userInfoModal").on('show.bs.modal', function (event) {
        var userId = $(event.relatedTarget).data('id');
        var userInfo = $("#" + userId +" td");

        $("#UserId").val(userId);
        $("#Name").val(userInfo.get(1).textContent.trim());
        $("#Email").val(userInfo.get(2).textContent.trim());
        $("#Birthday").val(userInfo.get(3).textContent.trim());
        $("#Telephone").val(userInfo.get(4).textContent.trim());
        $("#AccessRight").val(userInfo.get(5).dataset.accessNumber);
    });
}
