
$(function () {
    "use strict";

    var $table = $('#table');

    var columns = [
        { field: 'state', checkbox: true, align: 'center', valign: 'middle' },
        { field: 'operate', title: '操作', align: 'center', width: 40, formatter: operateFormatter },
        { field: 'Id', title: '主键', width: 40, sortable: true, align: 'center' },
        {
            field: 'Name', title: '配置名称', width: 150, sortable: false, align: 'center',
            editable: {
                type: 'text', title: '配置名称'
            }
        },
        {
            field: 'ParentName', title: '上级配置', width: 150, sortable: false, align: 'center'
        },
        //{ field: 'Guid', title: 'GUID', width: 280, sortable: false, align: 'center' },
        //{
        //    field: 'ParentGuid', title: '上级GUID', width: 280, sortable: false, align: 'center',
        //    editable: {
        //        type: 'text', title: '上级GUID'
        //    }
        //},
        {
            field: 'Code', title: '配置编码', width: 200, sortable: true, align: 'center',
            editable: {
                type: 'text', title: '配置编码'
            }
        },
        {
            field: 'ValueType', title: '值类型', width: 60, sortable: true, align: 'center',
            editable: {
                type: 'text', title: '配置值类型，比如int、string、bool'
            }
        },
        { field: 'Value', title: '配置值', sortable: false, align: 'left' },
        { field: 'Remark', title: '备注', sortable: false, align: 'left' },

        { field: 'Status', title: '状态', width: 40, sortable: true, align: 'center', formatter: statusFormatter },
        { field: 'Created_At', title: '发布日期', width: 140, sortable: true, align: 'center', formatter: formatterTime },
        { field: 'Updated_At', title: '修改日期', width: 140, sortable: true, align: 'center', formatter: formatterTime },
    ];
    var contextMenuItem = function (row, $el) {
        var item = $el.data('item');
        switch (item) {
            case 'edit':
                window.parent.addTab({ id: 'base-config-update-' + row.Id, href: '/admin/sys/base/config/update/' + row.Id, icons: 'fa fa-plus-circle m-r-xs', text: '修改配置' + row.Id });
                break;
            case 'publish':
                show.confirm(function () {
                    putStatus(statusModel.show, false, row.Id)
                });
                break;
            case 'trash':
                show.confirm(function () {
                    putStatus(statusModel.trash, false, row.Id)
                });
                break;
            case 'soldout':
                show.confirm(function () {
                    putStatus(statusModel.soldout, false, row.Id)
                });
                break;
            case 'delete':
                show.confirm(function () {
                    deleteRecords(false, row.Id)
                });
                break;
        }
    };
    var editableSave = function (field, row, oldValue, $el) {
        var update = function (field, value) {
            $.ajax({
                url: '?handler=UpdateField',
                type: "PUT",
                data: { id: row.Id, field: field, value: value },
                success: function (result) {
                    if (result.Code == 0) {
                        $table.bootstrapTable('refresh', { silent: true });
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
        switch (field) {
            case 'Name': update('Name', row.Name); break;
            case 'ParentGuid': update('ParentGuid', row.ParentGuid); break;
            case 'Code': update('Code', row.Code); break;
            case 'ValueType': update('ValueType', row.ValueType); break;
        }
    };

    createTable($table, {
        url: '?handler=PageList',
        columns: columns,
        queryParams: function (params) {
            params.status = $('input[type=radio][name=Status]:checked').val();
            params.code = $('input[type=radio][name=code]:checked').val();
            params.field = $('input[type=radio][name=field]:checked').val();
            params.search = $('#search').val();
            return params;
        },
        onContextMenuItem: contextMenuItem,
        onEditableSave: editableSave
    });


    $('.btnRefresh').click(function () {
        $table.bootstrapTable('refresh', { silent: true });
        return false;
    });
    $('#statusContainer li').on('click.bs.dropdown', function () {
        window.setTimeout(function () {
            $table.bootstrapTable('refresh', { silent: true });
        }, 50)
    })

    $('#search').keyup(function () {
        if (event.keyCode == 13) {
            $table.bootstrapTable('refresh', { silent: true });
        }
    });
    $('.dropdown-select > li > a').on('click', function () {
        setTimeout(function () {
            $table.bootstrapTable('refresh', { silent: true });
        }, 50);
    });
    var putStatus = function (status, batch, id) {
        var ids;
        if (batch) {
            ids = getSelections($table);
            if (ids.length == 0) {
                show.error('错误提示', '请选择要操作的数据');
                return false;
            }
        } else {
            ids = [id]
        }
        $.ajax({
            url: '?handler=BatchStatus',
            type: "PUT",
            data: { ids: ids, status: status },
            success: function (result) {
                if (result.Code == 0) {
                    $table.bootstrapTable('refresh', { silent: true });
                } else {
                    show.error('操作提示', result.Message);
                }
            }
        });
    }
    var deleteRecords = function (batch, id) {
        var ids;
        if (batch) {
            ids = getSelections($table);
            if (ids.length == 0) {
                show.error('错误提示', '请选择要永久删除的数据');
                return false;
            }
        } else {
            ids = [id]
        }
        $.ajax({
            url: '?handler=BatchDelete',
            type: "DELETE",
            data: { ids: ids },
            success: function (result) {
                if (result.Code == 0) {
                    $table.bootstrapTable('refresh', { silent: true });
                } else {
                    show.error('操作提示', result.Message);
                }
            }
        });
    }
    $('#btnPublish').click(function () {
        show.confirm(function () {
            putStatus(statusModel.show, true)
        });
        return false;
    });
    $('#btnTrash').click(function () {
        show.confirm(function () {
            putStatus(statusModel.trash, true)
        });
        return false;
    });
    $('#btnSoldOut').click(function () {
        show.confirm(function () {
            putStatus(statusModel.soldout, true)
        });
        return false;
    });

    $('#btnDelete').click(function () {
        show.confirm(function () {
            deleteRecords(true)
        });
        return false;
    });
});