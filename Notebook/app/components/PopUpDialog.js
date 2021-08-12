Vue.component('popup-dialog', function (resolve) {
    $.get('app/components/PopUpDialog.html', function (template) {
        resolve({
            props: {
                show: {
                    type: Boolean
                }

            },
            template: template,
            mounted() {
                let style = document.createElement('link');
                style.type = "text/css";
                style.rel = "stylesheet";
                style.href = 'app/components/PopUpDialog.css';
                document.head.appendChild(style);
            },
            methods: {
                hideDialog: function() {
                    this.$emit('update:show', false)
                }
            }
        })
    })
})