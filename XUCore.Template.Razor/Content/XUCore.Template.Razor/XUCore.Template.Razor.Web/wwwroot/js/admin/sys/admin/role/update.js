
$(function () {
    $('#data-form').parsleyForm({
        submit: function (formData) {
            formData['Role.Id'] = id;
            $.ajax({
                url: '?handler=RoleByUpdateRow',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('admin-role-update-' + id);
                                }
                            },
                        });
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });
});