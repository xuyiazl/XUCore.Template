$(function () {
    "use strict";

    var $table = $('#table');

    var columns = [
        {
            field: 'Title', title: '标题', sortable: false, align: 'left', formatter: function (val, row) {
                var html = '';
                html += "<a href='/admin/sys/admin/notices/detail/" + row.Id + "' target='_blank'><span class=\"" + row.LevelCss + "\">【" + row.LevelName + "】</span>" + row.Title + "</a>";
                html += '<span class="text-xs m-l">by ' + row.AdminName + ' ' + getDateDiff(row.Created_At) + '</span>';
                return html;
            }
        }
    ];
    createTable($table, {
        showHeader: false,
        url: '?handler=IndexNoticeList',
        columns: columns,
        queryParams: function (params) {
            return params;
        }
    });
});