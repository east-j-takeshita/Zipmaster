
    Vue.createApp({

        data() {
            return {
        title: '検索Vue',
    zipsData: @Html.Raw(Json.Serialize(Model.ZipsData)),
    url:'PostCodeDetail/'  
            }
        },
    methods: {
        msg() {
        alert('msg');
            }
        }
    

        
    }).mount('#view');
