<template>
    
        <h1 class="display-3">{{ title }}</h1>
        <form method="post" action="/PostCode/Index">
            @* 送信先のコントローラー名、送信先のアクション指定 *@
            <div class="form-group">
                <label for="postcode">郵便番号</label>
                <input type="text" name="postcode" id="postcode" class="form-control" placeholder="検索したい郵便番号を入力して検索してください" :value="keyWord.keyPostCode" />
                <label for="keyword">キーワード検索欄</label>
                <input type="text" name="keyword" id="keyword" class="form-control" placeholder="検索したいキーワードを入力して検索してください" :value="keyWord.keyWord" />

            </div>
            <div class="form-group">
                <input v-on:click="msg" type="submit" class="btn btn-primary" value="送信" />
            </div>
        </form>
        <p v-if="zipsData===null">該当データはありません</p>
        <table class="table">
            <tr>
                <th>郵便番号</th>
                <th>都道府県名</th>
                <th>市区町村名</th>
                <th>町域名</th>
                <th>詳細リンク</th>

            </tr>

            <tr v-for="zip in zipsData" :key="zip.id">

                <td>{{zip.postCode}}</td>
                <td>{{zip.prefecture}}</td>
                <td>{{zip.city}}</td>
                <td>{{zip.shipToAddress}}</td>
                <td><a v-bind:href="url+zip.postOrderID">詳細はこちら</a></td>
            </tr>

        </table>


        <button type="submit" class="btn btn-primary" @click="location.href ='PostCode/Create'" value="遷移" />新規追加



</template>

<script setup>
    import { ref, defineProps } from 'vue';
   
    const props = defineProps(['zipsDataProp', 'keywordProp']);
    const title = ref('検索Vue');
    const zipsData = ref(props.zipsDataProp);
    const keyWord = ref(props.keywordProp);
    const url = ref('PostCodeDetail/');

</script>