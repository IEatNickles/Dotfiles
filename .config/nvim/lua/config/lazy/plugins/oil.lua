return {
  "stevearc/oil.nvim",
  dependencies = {
    { "echasnovski/mini.icons", opts = {} }
  },
  config = function()
    require("oil").setup({
      keymaps = {
        ["<C-h>"] = false,
        ["<C-c>"] = false
      },
      view_options = {
        show_hidden = true
      }
    })
    vim.keymap.set("n", "-", "<CMD>Oil<CR>")
  end,
  lazy = false
}
