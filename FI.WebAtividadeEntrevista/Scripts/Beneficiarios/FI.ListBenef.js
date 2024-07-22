$(document).ready(function () {
    if (document.getElementById("gridBeneficiarios"))
        $('#gridBeneficiarios').jtable({
            title: 'Beneficiarios',
            paging: true,
            sorting: true,
            defaultSorting: 'Nome ASC',
            actions: {
                listAction: function (postData, jtParams) {
                    return $.Deferred(function ($dfd) {
                        $.ajax({
                            url: 'https://localhost:44333/Beneficiario/BeneficiarioList?idCliente=' + obj.Id + '&jtStartIndex=' + jtParams.jtStartIndex + '&jtPageSize=' + jtParams.jtPageSize + '&jtSorting=' + jtParams.jtSorting,
                            type: 'POST',
                            dataType: 'json',
                            data: postData,
                            success: function (data) {
                                $dfd.resolve(data);
                            },
                            error: function () {
                                $dfd.reject();
                            }
                        });
                    });
                }
            },
            fields: {
                Nome: {
                    title: 'Nome',
                    width: '50%'
                },
                CPF: {
                    title: 'CPF',
                    width: '35%'
                }
            }
        });

    if (document.getElementById("gridBeneficiarios"))
        $('#gridBeneficiarios').jtable('load');

    $('#includeButton').click(function (e) {
        e.preventDefault();

        var newRecordData = {
            // Collect data from your form inputs
            Nome: $('#Nome').val(),
            CPF: $('#CPF').val()
            // Add other fields as needed
        };

        $.ajax({
            url: 'https://localhost:44333/Beneficiario/AddBeneficiario', // Your add record URL
            type: 'POST',
            dataType: 'json',
            data: newRecordData,
            success: function (data) {
                if (data.Result === 'OK') {
                    $('#gridBeneficiarios').jtable('reload'); // Reload the table
                } else {
                    alert(data.Message);
                }
            },
            error: function () {
                alert('Error adding record');
            }
        });
    });
});
