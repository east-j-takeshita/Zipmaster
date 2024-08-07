const path = require('path');
const { VueLoaderPlugin } = require('vue-loader');

module.exports = {
    mode: 'development', // 'production'も選択可能
    entry: './Views/PostCode/src/app.js',
    output: {
        filename: 'bundle.js',
        path: path.resolve(__dirname, 'wwwroot/js'),
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader'
            },
            {
                test: /\.js$/,
                exclude: /(node_modules|bower_components)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }
            }
        ]
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm-bundler.js'
        },
        extensions: ['.js', '.vue']
    },
    plugins: [
        new VueLoaderPlugin()
    ]
};