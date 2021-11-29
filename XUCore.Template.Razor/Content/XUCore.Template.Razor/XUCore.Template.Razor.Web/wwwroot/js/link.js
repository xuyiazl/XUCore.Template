$(function () {

    $('#data-form').parsleyForm({
        submit: function (formData) {
            $.ajax({
                url: '/api/link/linkadd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.success('', result.Message);
                        $('#data-form')[0].reset();
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });
});