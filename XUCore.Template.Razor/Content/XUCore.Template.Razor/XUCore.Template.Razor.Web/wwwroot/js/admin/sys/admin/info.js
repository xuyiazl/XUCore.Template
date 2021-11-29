$(function () {

    $('#editorPicture').bindUpload({
        data: 'cutorgin=true&autocutsize=600&thumbs=300x300',
        success: function (res) {
            console.log(res.Data.Thumbs["300x300"])
            $.ajax({
                url: '?handler=UserUpdatePicture',
                type: "PUT",
                data: { Picture: res.Data.Thumbs["300x300"] },
                success: function (result) {
                    if (result.Code == 0) {
                        $('.thumb-lg img').attr('src', res.Data.Root + res.Data.Thumbs["300x300"]);
                    } else {
                        show.error('上传错误', result.Message);
                    }
                }
            });
        }
    });

    $('#password-form').parsleyForm({
        submit: function (formData) {
            var $oldpass = $('#txtOldPass');
            var $newpass = $('#txtNewPass');
            var $rePass = $('#txtRePass');
            $.ajax({
                url: '?handler=UserUpdatePassword',
                type: "PUT",
                data: { newpassword: $newpass.val(), oldpassword: $oldpass.val() },
                success: function (result) {
                    if (result.Code == 0) {
                        show.success('修改成功', '下次登录请使用新密码')
                        $newpass.val('');
                        $rePass.val('');
                        $oldpass.val('');
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });

    $('#info-form').parsleyForm({
        submit: function (formData) {
            $.ajax({
                url: '?handler=UserUpdateInfo',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.success('操作提示', '修改成功')
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });
});