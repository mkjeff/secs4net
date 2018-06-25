const path = require("path")

module.exports = {
  baseUrl: '/app/',
  outputDir: 'wwwroot/app',
  css: {
    extract: false,
  },

  chainWebpack: (config) => {
    config.plugins.delete('hmr');
    config.plugins.delete('pwa');
    config.plugins.delete('preload');
    config.plugins.delete('prefetch');
    config.plugins.delete('html');
    config.plugins.delete('copy');
    config.resolve.alias.set('@', path.resolve('ClientApp'));
    config.entryPoints.get('app').clear().add('./ClientApp/main.ts');
    config.output
      .filename('[name].js')
      .chunkFilename('[name].js');
  },
}