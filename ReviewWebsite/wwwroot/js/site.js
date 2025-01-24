// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    bindSideBarEvent();
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
            e.preventDefault();
            // 移除所有active状态
            sidebarLinks.forEach(l => l.classList.remove('active'));
            // 添加当前链接的active状态
            this.classList.add('active');
            const href = this.getAttribute('href')
            loadAjax(
                'GET',
                href,
                {},
                function (response) {   // 自定义错误处理
                  
                },
                function (error) {   // 自定义错误处理
                    console.log("error:" + error)
                }
            )
        });
    });
}

function loadAjax(type, href, data, onSuccess, onError,changeUrl=true) {
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
            const contentArea = document.getElementById('content-area');
            if (changeUrl) {
                // 插入內容
                history.pushState(null, '', href);
            }

            $('#content-area').html(response);

            // 動態加載 JS 和 CSS
            const styles = $("meta[name='dynamic-styles']").data("href");
            const scripts = $("meta[name='dynamic-scripts']").data("src");

            if (styles) {
                $('<link>')
                    .attr('rel', 'stylesheet')
                    .attr('href', styles)
                    .appendTo('head');
            }

            if (scripts) {
                $.getScript(scripts);
            }
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

function log(msg) {
    if (false) return false
    console.log(msg)
}