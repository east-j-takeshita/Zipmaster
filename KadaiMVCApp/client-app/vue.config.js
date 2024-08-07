const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
    outputDir: '../wwwroot/client-app',
    publicPath: '/',
    transpileDependencies: true
})
