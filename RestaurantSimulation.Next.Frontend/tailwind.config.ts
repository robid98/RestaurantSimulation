import { THEME_COLORS } from "./src/const/theme"

/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ["media"],
  content: [
    './pages/**/*.{ts,tsx}',
    './components/**/*.{ts,tsx}',
    './app/**/*.{ts,tsx}',
    './src/**/*.{ts,tsx}',
  ],
  theme: {
    container: {
      center: true,
      padding: "2rem",
      screens: {
        "2xl": "1400px",
      },
    },
    extend: {
      colors: {
        input: "hsl(var(--input))",
        ring: "hsl(var(--ring))",
        background: "hsl(var(--background))",
        foreground: "hsl(var(--foreground))",
        destructive: {
          DEFAULT: "hsl(var(--destructive))",
          foreground: "hsl(var(--destructive-foreground))",
        },
        muted: {
          DEFAULT: "hsl(var(--muted))",
          foreground: "hsl(var(--muted-foreground))",
        },
        accent: {
          DEFAULT: "hsl(var(--accent))",
          foreground: "hsl(var(--accent-foreground))",
        },
        popover: {
          DEFAULT: "hsl(var(--popover))",
          foreground: "hsl(var(--popover-foreground))",
        },
        card: {
          DEFAULT: "hsl(var(--card))",
          foreground: "hsl(var(--card-foreground))",
        },
        surface: {
          DEFAULT: THEME_COLORS.surface,
          dark: THEME_COLORS.surface_dark,
          foreground: THEME_COLORS.on_surface,
          'foreground-dark': THEME_COLORS.on_surface_dark
        },
        floating: {
          DEFAULT: THEME_COLORS.floating,
          dark: THEME_COLORS.floating_dark,
          foreground: THEME_COLORS.on_floating,
          'foreground-dark': THEME_COLORS.on_floating_dark
        },
        primary: {
          DEFAULT: THEME_COLORS.primary,
          dark: THEME_COLORS.primary_dark,
          foreground: THEME_COLORS.on_primary,
          'foreground-dark': THEME_COLORS.on_primary_dark
        },
        secondary: {
          DEFAULT: THEME_COLORS.secondary,
          dark: THEME_COLORS.secondary_dark,
          foreground: THEME_COLORS.on_secondary,
          'foreground-dark': THEME_COLORS.on_secondary_dark
        },
        border: {
          DEFAULT: THEME_COLORS.border,
          dark: THEME_COLORS.border_dark,
          dim: THEME_COLORS.border_dim,
          'dim-dark': THEME_COLORS.border_dim_dark
        },
        profits: {
          DEFAULT: THEME_COLORS.profits,
          dark: THEME_COLORS.profits_dark,
          foreground: THEME_COLORS.on_profits,
          'foreground-dark': THEME_COLORS.on_profits_dark
        },
        losses: {
          DEFAULT: THEME_COLORS.losses,
          dark: THEME_COLORS.losses_dark,
          foreground: THEME_COLORS.on_losses,
          'foreground-dark': THEME_COLORS.on_losses_dark
        }
      },
      borderRadius: {
        lg: "var(--radius)",
        md: "calc(var(--radius) - 2px)",
        sm: "calc(var(--radius) - 4px)",
      },
      keyframes: {
        "accordion-down": {
          from: { height: 0 },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: 0 },
        },
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out",
      },
      fontFamily: {
        tektur: 'Tektur'
      },
    },
  },
  plugins: [require("tailwindcss-animate")],
}