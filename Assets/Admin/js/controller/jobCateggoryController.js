var jobCategory = {
    init: function () {
        jobCategory.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();

            var btn = $(this);

            var id = btn.data('id');

            $.ajax({
                url: "/Admin/JobCategory/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (respone) {
                    if (respone.status == true) {
                        btn.text("Active");
                    } else {
                        btn.text("Unactive");
                    }
                }
            });
        })
    }
}
jobCategory.init();