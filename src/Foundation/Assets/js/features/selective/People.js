class Persons {
    constructor() {
        this.Init();
    }

    Init() {
        var instance = this;
        if ($('#persons').html().trim()) {
            $("#mainContentArea").hide();
        } else {
            $("#mainContentArea").show();
        }

        var options = {
            data: names,
            list: {
                match: {
                    enabled: true
                },
                onChooseEvent: function () {
                    var keyword = $("#txtName").getSelectedItemData();
                    $("#txtName").val(keyword);
                    $("#btnSearch").click();
                }
            }
        };

        $("#txtName").easyAutocomplete(options);

        $("#btnSearch").click(function () {
            instance.DoAjaxCallback();
        })

        $("#slSector").change(function () {
            $("#ulSector li").removeClass("active");
            $("[data-sector='" + $("#slSector").val() + "']").addClass("active");
        })

        $("#slLocation").change(function () {
            $("#ulLocation li").removeClass("active");
            $("[data-location='" + $("#slLocation").val() + "']").addClass("active");
        })

        $("#ulSector li").each(function () {
            $(this).click(function () {
                $("#ulSector li").removeClass("active");
                $(this).addClass("active");
                $("#slSector").val($(this).data("sector"));
                $("#btnSearch").click();
            })
        })

        $("#ulLocation li").each(function () {
            $(this).click(function () {
                $("#ulLocation li").removeClass("active");
                $(this).addClass("active");
                $("#slLocation").val($(this).data("location"));
                $("#btnSearch").click();
            })
        })
    }
    
    getFilterUrl() {
        var uri = new Uri(location.pathname);
        uri.replaceQueryParam("name", $("#txtName").val());
        uri.replaceQueryParam("sector", $("#slSector").val());
        uri.replaceQueryParam("location", $("#slLocation").val());
        return uri;
    }

    DoAjaxCallback() {
        let instance = this;
        $('.loading-box').show();
        axios.get(instance.getFilterUrl())
            .then(function (result) {
                var fetched = $(result.data);
                $('#persons').html(fetched.find('#persons').html());
                if ($('#persons').html().trim()) {
                    $("#mainContentArea").hide();
                }
                else {
                    $("#mainContentArea").show();
                }
            })
            .catch(function (error) {
                notification.Error(error);
            })
            .finally(function () {
                $('.loading-box').hide();
            });
    }

}
