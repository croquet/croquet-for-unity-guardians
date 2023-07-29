// invoked with an env.appName argument, which is used to determine the source
// and the output directories.
// NB: even if invoked from a different working directory, __dirname is the
// location of this config
const HtmlWebPackPlugin = require('html-webpack-plugin');
const path = require('path');

module.exports = env => ({
    entry: () => {
        // if custom index-node.js exists in the app directory, use it
        if (env.buildTarget === 'node') {
            try {
                const index = `../${env.appName}/index-node.js`;
                require.resolve(index);
                return index;
            } catch (e) {/* fall through and try index.js next */}
        }
        // if custom index.js exists in the app directory, use it
        try {
            const index = `../${env.appName}/index.js`;
            require.resolve(index);
            return index;
        } catch (e) {
            // otherwise, use the default index.tmp.js
            return `./sources/${env.buildTarget === 'node' ? 'index-node.tmp.js' : 'index.tmp.js'}`;
        }
    },
    output: {
        path: path.join(__dirname, `../../StreamingAssets/${env.appName}/`),
        pathinfo: false,
        filename: env.buildTarget === 'node' ? 'node-main.js' : 'index-[contenthash:8].js',
        chunkFilename: 'chunk-[contenthash:8].js',
        clean: true
    },
    cache: {
        type: 'filesystem',
        name: `${env.appName}-${env.buildTarget}`,
        buildDependencies: {
            config: [__filename],
        }
    },
    resolve: {
        fallback: { "crypto": false }
    },
    experiments: {
        asyncWebAssembly: true,
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                enforce: "pre",
                use: ["source-map-loader"],
            },
        ],
    },
    plugins: env.buildTarget === 'node' ? [] : [
        new HtmlWebPackPlugin({
            template: './sources/index.html',   // input
            filename: 'index.html',   // output filename in build
        }),
    ],
    externals: env.buildTarget !== 'node' ? [] : [
        {
            'utf-8-validate': 'commonjs utf-8-validate',
            bufferutil: 'commonjs bufferutil',
        },
    ],
    target: env.buildTarget || 'web'
});
