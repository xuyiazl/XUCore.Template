
$(function () {
    window.Parsley.addValidator('existmobile', {
        requirementType: 'string',
        validateString: function (value, requirement) {
            var response = false;
            if (!/^1[0-9][0-9]\d{4,8}$/.test(value)) {
                response = false;
            } else {
                var m_result = $.ajax({
                    type: "GET",
                    url: '?handler=UserExist',
                    data: { field: 'mobile', value: value },
                    async: false
                }).responseText;
                m_result = eval('(' + m_result + ')');
                response = m_result.Code == 0;
            }
            return response;
        },
        messages: {
            'zh-cn': '手机号码不可用',
        }
    });
    window.Parsley.addValidator('existusername', {
        requirementType: 'string',
        validateString: function (value, requirement) {
            var m_result = $.ajax({
                type: "GET",
                url: '?handler=UserExist',
                data: { field: 'username', value: value },
                async: false
            }).responseText;
            m_result = eval('(' + m_result + ')');
            return m_result.Code == 0;
        },
        messages: {
            'zh-cn': '帐号不可用',
        }
    });

    $('#data-form').parsleyForm({
        submit: function (formData) {
            $.ajax({
                url: '?handler=UserByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('admin-user-add');
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