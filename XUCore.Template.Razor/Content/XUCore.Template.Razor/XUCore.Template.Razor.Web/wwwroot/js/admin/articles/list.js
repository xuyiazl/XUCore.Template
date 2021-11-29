
$(function () {
    "use strict";

    var $table = $('#table');

    var columns = [
        { field: 'state', checkbox: true, align: 'center', valign: 'middle' },
        { field: 'operate', title: '操作', align: 'center', width: 40, formatter: operateFormatter },
        {
            field: 'CategoryName', title: '目录', width: 120, sortable: true, align: 'left', formatter: function (val, row) {
                return '<span class="' + row.CategoryCss + '" >' + row.CategoryName + '</span>';
            }
        },
        {
            field: 'Title', title: '标题', sortable: false, align: 'left', formatter: function (val, row) {
                return "<a href='/blog/" + row.Id + "' target='_blank'>" + row.Title + "</a>";
            }
        },
        {
            field: 'CommentCount', title: '评论数', sortable: true, align: 'center', width: 40, formatter: function (val, row) {
                return row.CommentCount + row.CommentReplyCount
            }
        },
        {
            field: 'Weight', title: '权重', sortable: true, align: 'center', width: 40,
            editable: {
                type: 'text', title: '权重', placement: 'left', validate: function (value) {
                    value = $.trim(value);
                    if (value) {
                        if (!/^([1-9]\d*|[0]{1,1})$/.test(value))
                            return '只能输入整数!';
                    }
                }
            }
        },
        { field: 'Status', title: '状态', width: 40, sortable: true, align: 'center', formatter: statusFormatter },
        { field: 'Created_At', title: '发布日期', width: 140, sortable: true, align: 'center', formatter: formatterTime },
        { field: 'AdminName', title: '发布人', width: 80, sortable: true, align: 'center' },
        { field: 'Updated_At', title: '修改日期', width: 140, sortable: true, align: 'center', formatter: formatterTime }
    ];
    var contextMenuItem = function (row, $el) {
        var item = $el.data('item');
        switch (item) {
            case 'edit':
                if (row.Editor == 'tinymce')
                    window.parent.addTab({ id: 'article-update-tinymce-' + row.Id, href: '/admin/articles/update/tinymce/' + row.Id, icons: 'fa fa-plus-circle m-r-xs', text: '修改Tinymce文章' + row.Id });
                else
                    window.parent.addTab({ id: 'article-update-md-' + row.Id, href: '/admin/articles/update/md/' + row.Id, icons: 'fa fa-plus-circle m-r-xs', text: '修改Markdown文章' + row.Id });
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
            case 'Title': update('Title', row.Title); break;
            case 'Weight': update('Weight', row.Weight); break;
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
});