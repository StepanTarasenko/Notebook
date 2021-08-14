Vue.component('popup-dialog', function (resolve) {
    $.get('app/components/PopUpDialog.html', function (template) {
        resolve({
            props: {
                show: {
                    type: Boolean
                }
            },
            template: template,
            methods: {
                hideDialog: function() {
                    this.$emit('update:show', false);
                }
            }
        })
    })
})