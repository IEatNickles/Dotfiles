return {
  "williamboman/mason.nvim",
  dependencies = {
    "mason-org/mason-lspconfig.nvim",
    "zapling/mason-conform.nvim"
  },
  lazy = false,
  init = function()
    require("mason").setup()
    require("mason-lspconfig").setup()
    require("mason-conform").setup()
  end,
}
