return {
  'jakemason/ouroboros',
  dependencies = { 'nvim-lua/plenary.nvim' },
  config = function()
    local extension_preferences_table = {
      c = { h = 2, hpp = 1 },
      h = { c = 2, cpp = 1 },
      cpp = { hpp = 2, h = 1 },
      hpp = { cpp = 2, c = 3, inl = 1 },

      inl = { hpp = 2 },
    }
    require('ouroboros').setup({
      extension_preferences_table = extension_preferences_table,
      -- if this is true and the matching file is already open in a pane, we'll
      -- switch to that pane instead of opening it in the current buffer
      switch_to_open_pane_if_possible = true,
    })
    vim.keymap.set("n", "<C-e>", function()
      if extension_preferences_table[vim.bo.filetype] ~= nil then
        vim.cmd(":Ouroboros")
      else
        vim.print("not a c or cpp file")
      end
    end)
  end
}
