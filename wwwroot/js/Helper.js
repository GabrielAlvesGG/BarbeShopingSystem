$(document).ready(function () {

    var input = $('#cpf-cnpj');

    input.on('input', function () {
        var value = input.val().replace(/\D/g, ''); 
        var caret = this.selectionStart; 

        
        if (value.length > 11) {
            input.unmask().mask('00.000.000/0000-00');
        } else {
            input.unmask().mask('000.000.000-009'); 
        }

        this.setSelectionRange(caret, caret);
    });

    // Para telefone
    $("#phone").mask("(00) 00000-0000");
});