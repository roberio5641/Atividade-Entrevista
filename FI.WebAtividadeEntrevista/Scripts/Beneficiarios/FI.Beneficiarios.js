$(document).ready(function () {
    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();
        console.log(obj.Id)
        $.ajax({
            url: "https://localhost:44333/Beneficiario/Incluir", 
            method: "POST",
            data: {
                "Nome": $(this).find("#Nome").val(),
                "CPF": $(this).find("#CPF").val(),
                "IdCliente": obj.Id 
            },
            error: function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success: function (r) {
                ModalDialog("Sucesso!", r);
                $("#formBeneficiario")[0].reset();
            }
        });
    });
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var dialogHtml = '<div id="' + random + '" class="modal fade">' +
        '        <div class="modal-dialog">' +
        '            <div class="modal-content">' +
        '                <div class="modal-header">' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>' +
        '                    <h4 class="modal-title">' + titulo + '</h4>' +
        '                </div>' +
        '                <div class="modal-body">' +
        '                    <p>' + texto + '</p>' +
        '                </div>' +
        '                <div class="modal-footer">' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>' +
        '                </div>' +
        '            </div>' +
        '        </div>' +
        '    </div>';

    $('body').append(dialogHtml);
    $('#' + random).modal('show');
}
