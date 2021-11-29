$(function () {
    "use strict";

    var $table = $('#table');

    createTable($table, {
        columns: [
            { field: 'UserName', title: '帐号', sortable: false, align: 'center' },
            { field: 'Mobile', title: '手机', sortable: false, align: 'center' },
            { field: 'Name', title: '姓名', sortable: false, align: 'center' },
            { field: 'LoginWay', title: '登录方式', sortable: true, align: 'center' },
            { field: 'LoginIp', title: '登录IP', sortable: false, align: 'center' },
            { field: 'LoginTime', title: '登录时间', sortable: true, align: 'center', formatter: formatterTime }
        ],
        queryParams: function (params) {
            params.field = $('input[type=radio][name=field]:checked').val();
            params.search = $('#search').val();
            return params;
        },
        url: '?handler=RecordList',
        sortName: 'LoginTime',
        sortOrder: 'desc'
    });

    $('.btnRefresh').click(function () {
        $table.bootstrapTable('refresh', { silent: true });
        return false;
    });
    $('#search').keyup(function () {
        if (event.keyCode == 13) {
            $table.bootstrapTable('refresh', { silent: true });
        }
    });
});