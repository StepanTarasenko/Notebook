Vue.component('popup-container', function (resolve) {
    $.get('app/components/PopUpСontainer.html', function (template) {
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