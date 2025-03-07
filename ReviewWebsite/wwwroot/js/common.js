$(function () {
    bindAllEvent();
});

function bindAllEvent() {
    $("#selectable").on("mousedown", function (e) {
        e.metaKey = true;
    }).selectable();
}
