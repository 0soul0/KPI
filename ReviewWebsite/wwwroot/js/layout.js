var cookieUser=null
$(function () {
    setSetting();
    bindSideBarEvent();
    bindCompentEvent();
    // 处理浏览器前进/后退按钮
    window.addEventListener("popstate", function () {
        const href = window.location.pathname;
        loadContent(href, {}, "content");
    });
});

function bindSideBarEvent() {
    const sidebarLinks = document.querySelectorAll('.sidebar .nav-item');
    // 处理链接点击事件
    sidebarLinks.forEach(link => {
        link.addEventListener('click', function (e) {

            const href = this.getAttribute('href')
            if (href == "") return

            e.preventDefault();
            // 移除所有active状态
            sidebarLinks.forEach(l => l.classList.remove('active'));
            // 添加当前链接的active状态
            this.classList.add('active');
            loadAjax(
                'GET',
                href,
                {},
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
        });
    });
}

function bindCompentEvent() {
    $(".selectable_item").on('click', function () {
        $(this).toggleClass("selected_item");

    });

    $("#toggle").on('click', function () {
        $(this).toggleClass("rotated");
    })
}

function setSetting() {
    if ($.cookie('token') != "") {
        cookieUser = $.parseJSON(atob($.cookie('token')))
        $("#userName").text("嗨," + cookieUser.FullName)
    }
    $("#logOut").on('click', function () {
        $.cookie('token', '')
        cookieUser =""
        location.reload()
    })
}