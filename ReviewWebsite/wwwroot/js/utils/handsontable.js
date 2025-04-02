
$(function () {
});
/**
 * 4. 舊新資料比較
 */

function createExcelTable(data, containerId, afterRender, onCreateDone) {
    this.containerId = containerId
    const container = document.getElementById(containerId);
    container.innerHTML = ''
    var colspan = data[0].length
    var cellWidths = calculateCellWidth(colspan, (container.clientWidth - 68))
    handsontable = new Handsontable(container, {
        data: data,
        width: '100%',
        height: 'auto',
        colWidths(index) {
            if (index < cellWidths.length) {
                return cellWidths[index]; 
            }
            return cellWidths[cellWidth.length - 1]; 
          
        },
        colHeaders: true,
        rowHeaders: true,
        contextMenu: {
            items: {
                "row_above": { name: '插入上一行' },
                "row_below": { name: '插入下一行' },
                "remove_row": { name: '刪除行' },
                "remove_col": { name: '刪除列' },
                //"merge_cells": { name: '合併儲存格' },
                //"unmerge_cells": { name: '取消合併儲存格' },
                // "col_left": { name: '插入左邊欄' },  
                // "col_right": { name: '插入右邊欄' },
            }
        },  // 右鍵功能表
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
        filters: true,  // 開啟過濾器
        cells: function (row, col) {
            if (row === 0 && (col === 0 || col === 1)) {//年分
                return {
                    className: 'fw-bold'
                };
            }
            if (row === 1) {//title
                return {
                    readOnly: true,
                    className: 'fw-bold bg-primary text-dark bg-opacity-10'
                };
            }
            if (col === 5) { //kpi
                return {
                    readOnly: true,
                    className: 'select_kpi'
                };
            }
            if (col === 2) { //總分
                return {
                    type: 'dropdown', // 設定為下拉選單
                    source: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10'], // 下拉選單選項
                    strict: true
                };
            }
            if (col > 5) { //總分
                return {
                    type: 'dropdown', // 設定為下拉選單
                    strict: true
                };
            }
        },
        mergeCells: [
            { row: 0, col: 1, rowspan: 1, colspan: colspan - 1 }// 合併從 B1 到 Z1，假設有 26 列（A 到 Z）
        ],
        afterRender: function (changes) {
            afterRender(changes)
        },
        licenseKey: 'non-commercial-and-evaluation',
    });
    bindTableEvent(handsontable, containerId)
    if (typeof onCreateDone === 'function') {
        onCreateDone(handsontable);
    }
    return handsontable
}

function calculateCellWidth(colspan, clientWidth) {
    var defaultPercent = 0.063478261; // 預設新增的比例
    var cellWidthPercent = [0.152173913, 0.304347826, 0.043478261, 0.173913043, 0.217391304, 0.108695652]
    while (cellWidthPercent.length < colspan) {
        cellWidthPercent.push(defaultPercent);
    }
    
    var totalWidth = cellWidthPercent
        .map(value => value * clientWidth)
        .reduce((sum, width) => sum + width, 0);

    return cellWidthPercent
        .map(value => value * clientWidth * clientWidth / totalWidth)

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
    var height = $('.wtHider').eq(0).height() + 35
    $("#" + containerId).height(height);

}