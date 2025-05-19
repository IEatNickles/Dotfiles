return {
  'saghen/blink.cmp',
  version = 'v0.*',

  opts = {
    keymap = {
      preset = 'default',
      ['<C-s>'] = { 'show', 'show_documentation', 'hide_documentation' },
      ['<C-e>'] = { 'hide' },
      ['<C-y>'] = { 'select_and_accept' },

      ['<C-k>'] = { 'select_prev', 'fallback' },
      ['<C-j>'] = { 'select_next', 'fallback' },

      ['<C-b>'] = { 'scroll_documentation_up', 'fallback' },
      ['<C-f>'] = { 'scroll_documentation_down', 'fallback' },

      ['<Tab>'] = { 'snippet_forward', 'fallback' },
      ['<S-Tab>'] = { 'snippet_backward', 'fallback' },
    },

    appearance = {
      use_nvim_cmp_as_default = true,
    },

    signature = { enabled = true },

    completion = {
      ghost_text = { enabled = true },
      documentation = {
        draw = function(opts)
          if opts.item and opts.item.documentation then
            local out = require("pretty_hover.parser").parse(opts.item.documentation.value)
            opts.item.documentation.value = out:string()
          end

          opts.default_implementation(opts)
        end,
      }
    },

    sources = {
      default = { "lazydev", "lsp", "path", "buffer" },
      providers = {
        lazydev = {
          name = "LazyDev",
          module = "lazydev.integrations.blink",
          -- make lazydev completions top priority (see `:h blink.cmp`)
          score_offset = 100,
        },
      },
    }
  },
}
