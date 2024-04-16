import Header from "./components/Header";
import Shop from "./components/Shop";
import { DUMMY_PRODUCTS } from "./data/dummy-products.js";
import Product from "./components/Product.jsx";
import ShoppingCartContextProvider from "./stores/shopping-cart-context.jsx";

export default function App() {
  return (
    <ShoppingCartContextProvider>
      <Header />
      <Shop>
        {DUMMY_PRODUCTS.map((product) => (
          <li key={product.id}>
            <Product {...product} />
          </li>
        ))}
      </Shop>
    </ShoppingCartContextProvider>
  );
}
