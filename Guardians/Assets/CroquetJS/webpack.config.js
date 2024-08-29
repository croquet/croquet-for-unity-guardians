/* eslint import/no-extraneous-dependencies: ["error", {"devDependencies": true}] */
const CopyPlugin = require('copy-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const RemovePlugin = require('remove-files-webpack-plugin');
const webpack = require('webpack');
const path = require('path');
const fs = require('fs');


module.exports = (env) => {
    const isWebGL = env.buildTarget === 'webgl';
    const webGLPath = path.join(__dirname, `../WebGLTemplates/CroquetLoader/`);
    const nonWebGLPath = path.join(__dirname, `../StreamingAssets/${env.appName}/`);
    const destination = isWebGL ? webGLPath : nonWebGLPath;

    const lobbyDir = path.join(__dirname, `./lobby`);
    const withLobby = isWebGL && fs.existsSync(lobbyDir);

    // console.log(`Building for ${env.buildTarget}, ${withLobby?'with':'without'} lobby`);

    return {
        // infrastructureLogging: {
        //     level: 'verbose',
        // },
        devtool: 'source-map',
        entry: () => {
            // if this is a node build, look for index-node.js in the app directory
            if (env.buildTarget === 'node') {
                try {
                    const index = `./${env.appName}/index-node.js`;
                    require.resolve(index); // throws if not found
                    return {index};
                } catch (e) {/* fall through and try index.js next */}
            }
            // otherwise (or if index-node not found) assume there is an index.js
            const index = `./${env.appName}/index.js`;
            require.resolve(index);
            // if this is a WebGL build, and there is a lobby, use both
            if (withLobby) {
                try {
                    const lobby = `./lobby/lobby.js`;
                    require.resolve(lobby);
                    return { game: index, lobby };
                } catch (e) { /* fall through and use index.js */}
            }
            return {index};
        },
        output: {
            path: destination,
            pathinfo: false,
            filename: env.buildTarget === 'node' ? 'node-main.js' : '[name]-[contenthash:8].js',
            // filename: env.buildTarget === 'node' ? 'node-main.js' : '[name]_croquet.js',
            chunkFilename: 'chunk-[contenthash:8].js',
            clean: !isWebGL // RemovePlugin below handles index-####.js files in WebGL
        },
        cache: {
            type: 'filesystem',
            name: `${env.appName}-${env.buildTarget}${isWebGL?'-WebGL':''}`,
            buildDependencies: {
                config: [__filename],
            }
        },
        resolve: {
            fallback: {
                "crypto": false,
                ...(isWebGL ? {
                    "buffer": require.resolve("buffer/"),
                    "stream": require.resolve("stream-browserify"),
                    "assert": require.resolve("assert/"),
                } : {})
            }
        },
        module: {
            rules: [
                isWebGL && {
                    test: /\.js$/,
                    exclude: /node_modules/,
                    use: {
                        loader: 'babel-loader',
                        options: {
                            presets: ['@babel/preset-env'],
                            plugins: ['@babel/plugin-transform-modules-commonjs']
                        }
                    }
                },
                {
                    test: /\.js$/,
                    enforce: "pre",
                    use: ["source-map-loader"],
                },
            ].filter(x => x), // removes any undefined by the && predicate being false
        },
        plugins: [
            isWebGL && new RemovePlugin({
                before: {
                    allowRootAndOutside: true,
                    test: [
                        {
                            // remove app-specific content from StreamingAssets
                            folder: nonWebGLPath,
                            method: _absolutePath => true,
                            recursive: true
                        },
                        {
                            // in build dir, remove old hashed .js files and their meta files
                            folder: webGLPath,
                            // method: absolutePath => /(index|lobby|main|game)_croquet.js/m.test(absolutePath)
                            method: absolutePath => /(index|lobby|main|game)-.+\.js/m.test(absolutePath)
                                || (!withLobby && /game.html/m.test(absolutePath)),
                        }
                    ],
                    log: false
                }
            }),
            new CopyPlugin({
                patterns: [
                    {
                        from: `./${env.appName}/scene-definitions.txt`,
                        to: `${destination}/scene-definitions.txt`,
                        noErrorOnMissing: true
                    }
                ]
            }),
            // build main html if not building for node
            env.buildTarget !== 'node' && new HtmlWebpackPlugin({
                template: isWebGL
                    ? './_Runtime/Platforms/WebGL/index.html'
                    : './_Runtime/Platforms/WebView/webview.html',
                filename: withLobby ? 'game.html' : 'index.html',
                chunks: ['index', 'game'], // main chunk is either index or game
                inject: 'body',
            }),
            withLobby && new HtmlWebpackPlugin({ // adds lobby.html to the build
                template: `${lobbyDir}/lobby.html`,
                filename: 'index.html',
                inject: 'body',
                chunks: ['lobby'],
            }),
            isWebGL && new webpack.ProvidePlugin({
                Buffer: ['buffer', 'Buffer'],
            }),
            isWebGL && new webpack.ProvidePlugin({
                process: 'process/browser',
            }),
        ].filter(x => x), // removes any undefined by the && predicate being false
        externals: env.buildTarget !== 'node' ? [] : [
            {
                'utf-8-validate': 'commonjs utf-8-validate',
                bufferutil: 'commonjs bufferutil',
            },
        ],
        target: env.buildTarget !== 'node' ? 'web' : 'node',
        experiments: {
            outputModule: isWebGL,
            asyncWebAssembly: true,
        }
    };
};
