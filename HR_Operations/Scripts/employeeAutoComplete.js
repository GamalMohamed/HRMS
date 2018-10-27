$(document).ready(function () {
    $("#MajorUniversity").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/MajorUniversityAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.MajorUniversity, value: item.MajorUniversity };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#Subsidiary").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/SubsidiaryAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Subsidiary, value: item.Subsidiary };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#Vendor").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/VendorAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Vendor, value: item.Vendor };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#BasedOut").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/BasedOutAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.BasedOut, value: item.BasedOut };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#ReportingManager").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/ReportingManagerAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name, value: item.Name };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#PreviousEmployer").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/PreviousEmployerAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.PreviousEmployer, value: item.PreviousEmployer };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

    $("#Movement").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Employees/MovementAutoComplete",
                type: "POST",
                dataType: "json",
                data: { prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Movement, value: item.Movement };
                    }));
                }
            });
        },
        messages: {
            noResults: "", results: ""
        }
    });

})