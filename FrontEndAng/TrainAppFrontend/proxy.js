const PROXY_CONFIG = [      {
      context: [
          "/api",
          "/ohmioapi"
      ],

      target: "http://localhost:54918",        
      changeOrigin: true,
      logLevel: "debug"

   }    ]

module.exports = PROXY_CONFIG;