
$(function () {
});
/**
 * 1. 全可編輯
 * 2. 單位可編輯 全單位下拉分數 連動kpi 
 * 3. 全不可編輯
 * 4. 舊新資料比較
 * 5. cell 寬固定
 * 5. 串接使用者
 * 6. 頁面刷新進度
 */
function createExcelTable(data, containerId, afterRender, onCreateDone) {
    this.containerId = containerId
    const container = document.getElementById(containerId);
    container.innerHTML = ''
    var colspan = data[0].length - 1
    handsontable = new Handsontable(container, {
        data: data,
        colHeaders: true,
        rowHeaders: true,
        contextMenu: {
            items: {
                "row_above": { name: '插入上一行' },
                "row_below": { name: '插入下一行' },
                "remove_row": { name: '刪除行' },
                "remove_col": { name: '刪除列' },
                "merge_cells": { name: '合併儲存格' },
                "unmerge_cells": { name: '取消合併儲存格' },
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
            if (col === 2) { //總分
                return {
                    type: 'dropdown', // 設定為下拉選單
                    source: ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10'], // 下拉選單選項
                    strict: true
                };
            }
            if (col === 5) { //kpi
                return {
                    readOnly: true,
                    className: 'select_kpi' 
                };
            }


            //const cellProperties = {};
            //if (row === 0 && (col === 0 || col === 1)) {
            //    cellProperties.className = "htBold";
            //}
            //if (row === 1) {
            //    cellProperties.readOnly = true;
            //    cellProperties.className = 'htGray';
            //}
            //return cellProperties;
        },
        mergeCells: [
            { row: 0, col: 1, rowspan: 1, colspan: colspan }// 合併從 B1 到 Z1，假設有 26 列（A 到 Z）
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