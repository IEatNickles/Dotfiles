return {
	"neovim/nvim-lspconfig",
	dependencies = {
		"williamboman/mason-lspconfig.nvim",
		{ "antosha417/nvim-lsp-file-operations", config = true },
	},
	event = { "BufReadPre", "BufNewFile" },
	lazy = false,
	config = function()
		local telescope = require("telescope.builtin")

		local mason_lspconfig = require("mason-lspconfig")

		vim.api.nvim_create_autocmd("LspAttach", {
			group = vim.api.nvim_create_augroup("UserLspConfig", {}),
			callback = function(ev)
				local opts = { buffer = ev.buf, silent = true }

				vim.keymap.set("n", "gr", telescope.lsp_references, opts)
				vim.keymap.set("n", "gd", telescope.lsp_definitions, opts)
				vim.keymap.set("n", "gi", telescope.lsp_implementations, opts)
				vim.keymap.set("n", "gt", telescope.lsp_type_definitions, opts)
				vim.keymap.set("n", "<leader>ca", vim.lsp.buf.code_action, opts)
				vim.keymap.set("n", "<leader>rn", vim.lsp.buf.rename, opts)
				vim.keymap.set("n", "<leader>D", telescope.diagnostics, opts)
				vim.keymap.set("n", "<leader>d", vim.diagnostic.open_float, opts)
				vim.keymap.set("n", "K", vim.lsp.buf.hover, opts)
				vim.keymap.set("n", "<leader>rs", ":LspRestart<CR>", opts)
			end,
		})

		vim.diagnostic.config({
			signs = {
				text = {
					[vim.diagnostic.severity.ERROR] = "X",
					[vim.diagnostic.severity.WARN] = "!",
					[vim.diagnostic.severity.HINT] = "?",
					[vim.diagnostic.severity.INFO] = "-",
				},
				-- linehl = {
				--   [vim.diagnostic.severity.ERROR] = "DiagnosticSignError",
				--   [vim.diagnostic.severity.WARN] = "DiagnosticSignWarn",
				--   [vim.diagnostic.severity.HINT] = "DiagnosticSignHint",
				--   [vim.diagnostic.severity.INFO] = "DiagnosticSignInfo",
				-- }
			},
		})

		-- local pid = vim.fn.getpid()
		-- local omnisharp_bin = "/usr/local/bin/omnisharp-roslyn/OmniSharp"

		vim.lsp.config("omnisharp", {
			cmd = { "omnisharp-mono", "--languageserver", "--hostPID", tostring(vim.fn.getpid()) },
			filetypes = { "cs" },
			-- root_dir = vim.lsp.util.root_pattern("*.csproj", "*.sln", ".git"),
			settings = {
				useModernNet = false,
				monoPath = vim.fn.system({ "which", "mono" }),
			},
		})

		local capabilities = require("blink.cmp").get_lsp_capabilities()
		mason_lspconfig.setup({
			function(server_name)
				vim.lsp.config(server_name, {
					capabilities = capabilities,
				})
			end,
		})
	end,
}
