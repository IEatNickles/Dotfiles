return {
  'stevearc/conform.nvim',
  opts = {},
  dependencies = {
    "zapling/mason-conform.nvim"
  },
  init = function()
    require("mason-conform").setup()
    require("conform").setup({
      format_on_save = {
        -- These options will be passed to conform.format()
        timeout_ms = 500,
        lsp_format = "fallback",
      },
    })
  end
}
