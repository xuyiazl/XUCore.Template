
$(function () {
    "use strict";

    var $table = $('#table');

    var columns = [
        { field: 'state', checkbox: true, align: 'center', valign: 'middle' },
        { field: 'operate', title: '操作', align: 'center', width: 40, formatter: operateFormatter },
        {
            field: 'Name', title: '姓名', sortable: true, align: 'center',
            editable: {
                type: 'text', title: '姓名'
            }
        },
        {
            field: 'UserName', title: '账号', sortable: false, align: 'center',
            editable: {
                type: 'text', title: '账号', validate: function (v) {
                    if (!v || v.length == 0)
                        return '账号不允许为空';
                }
            }
        },
        {
            field: 'Mobile', title: '手机号码', sortable: false, width: 120, align: 'center',
            editable: {
                type: 'text', title: '手机号码', validate: function (v) {
                    if (!/^1[0-9][0-9]\d{4,8}$/.test(v))
                        return '手机号码格式错误';
                }
            }
        },
        {
            field: 'Password', title: '密码', sortable: false, width: 80, align: 'center',
            editable: {
                type: 'text', title: '重置密码', display: function (val, row) {
                    $(this).html('点击修改');
                }
            }
        }
        , {
            field: 'Location', title: '所在地', sortable: false, width: 120, align: 'center',
            editable: {
                type: 'text', title: '职位'
            }
        }
        , {
            field: 'Position', title: '职位', sortable: false, width: 120, align: 'center',
            editable: {
                type: 'text', title: '职位'
            }
        }
        , {
            field: 'Company', title: '公司', sortable: false, width: 120, align: 'center',
            editable: {
                type: 'text', title: '公司'
            }
        }
        , { field: 'LoginCount', title: '登录次数', sortable: false, width: 40, align: 'center' }
        , { field: 'LoginLastTime', title: '最后登录时间', sortable: true, width: 140, align: 'center', formatter: formatterTime}
        , { field: 'LoginLastIp', title: '最后登录IP', sortable: false, width: 140, align: 'center' }
        ,
        { field: 'Status', title: '状态', width: 40, sortable: true, align: 'center', formatter: statusFormatter },
        { field: 'Created_At', title: '发布日期', width: 140, sortable: true, align: 'center', formatter: formatterTime},
        { field: 'Updated_At', title: '修改日期', width: 140, sortable: true, align: 'center', formatter: formatterTime},
    ];
    var contextMenuItem = function (row, $el) {
        var item = $el.data('item');
        switch (item) {
            case 'accredit':
                {
                    $.get('?handler=AccreditList', { id: row.Id }, function (res) {
                        $('input[name=Roles]', $roleForm).iCheck('uncheck').each(function () {
                            var _this = $(this);
                            for (var i in res) {
                                var item = res[i];
                                if (_this.val() == item)
                                    _this.iCheck('check');
                            }
                        });
                        $('.modal-title', $roleModal).html('<i class="fa fa-bullhorn"></i>&nbsp;更改《' + row.Name + '》的权限');
                        $roleModal.data('id', row.Id);
                        $roleModal.modal('show');
                    });
                }
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
            case 'UserName': update('UserName', row.UserName); break;
            case 'Mobile': update('Mobile', row.Mobile); break;
            case 'Password': update('Password', row.Password); break;
            case 'Position': update('Position', row.Position); break;
            case 'Location': update('Location', row.Location); break;
            case 'Company': update('Company', row.Company); break;
        }
    };

    createTable($table, {
        url: '?handler=PageList',
        columns: columns,
        queryParams: function (params) {
            params.status = $('input[type=radio][name=Status]:checked').val();
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

    var $roleModal = $('#role-modal');
    var $roleForm = $('#role-form');

    $roleForm.parsleyForm({
        submit: function (formData) {
            formData['id'] = $roleModal.data('id');
            $.ajax({
                url: '?handler=Accredit',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        $roleModal.data('id', 0);
                        $roleModal.modal('hide');
                    } else {
                        show.error("操作提示", result.Message);
                    }
                }
            });
        }
    });

});