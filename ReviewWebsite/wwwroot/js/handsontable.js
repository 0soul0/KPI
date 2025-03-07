$(function () {
});

let handsontable
function createTable(data, containerId) {
    this.containerId = containerId
    const container = document.getElementById(containerId);
    container.innerHTML = ''
    handsontable = new Handsontable(container, {
        data: data,
        colHeaders: true,
        rowHeaders: true,
        contextMenu: true,  // 右鍵功能表
        formulas: true,  // 支援 Excel 公式
        formulas: {
            engine: HyperFormula
        },
        autoWrapRow: true,
        autoWrapCol: true,
        manualRowMove: true,
        manualColumnResize: true,
        minRows: 10,
        minCols: 6,
        //cells: function (row, col) {
        //    const cellProperties = {};
        //    if (row === 0 && (col === 0 || col === 1)) {
        //        cellProperties.className = "htBold";
        //    }
        //    if (row === 1) {
        //        cellProperties.readOnly = true;
        //        cellProperties.className = 'htGray';
        //    }
        //    return cellProperties;
        //},
        //mergeCells: [
        //    { row: 0, col: 1, rowspan: 1, colspan: 5 }
        //],
        licenseKey: 'non-commercial-and-evaluation',
    });
    bindTableEvent(handsontable, containerId)
}

function bindTableEvent(hot, containerId) {
    adjustContainerHeight(containerId);
    hot.addHook('afterChange', function (changes, source) {
        adjustContainerHeight(containerId);
    });

    // 確保每次渲染後調整容器高度
    hot.addHook('afterRender', function () {
        adjustContainerHeight(containerId);
    });

}

function adjustContainerHeight(containerId) {
    var height = $('.wtHider').eq(0).height()+35
    $("#" + containerId).height(height);

}