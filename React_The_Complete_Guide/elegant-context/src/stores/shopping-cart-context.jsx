import { createContext, useReducer } from "react";
import { DUMMY_PRODUCTS } from "../data/dummy-products";

export const ShoppingCartContext = createContext({
  items: [],
  addItemToCart: () => {},
  updateCartItemQuantity: () => {},
});

const shoppingCartReducer = (state, action) => {
  switch (action.type) {
    case "ADD_ITEM":
      return addItemToCart(state, action);

    case "UPDATE_ITEM":
      return updateCartItemQuantity(state, action);

    default:
      break;
  }
};

const addItemToCart = (state, action) => {
  const updatedItems = [...state.items];

  const existingCartItemIndex = updatedItems.findIndex(
    (cartItem) => cartItem.id === action.payload
  );
  const existingCartItem = updatedItems[existingCartItemIndex];

  if (existingCartItem) {
    const updatedItem = {
      ...existingCartItem,
      quantity: existingCartItem.quantity + 1,
    };
    updatedItems[existingCartItemIndex] = updatedItem;
  } else {
    const product = DUMMY_PRODUCTS.find(
      (product) => product.id === action.payload
    );
    updatedItems.push({
      id: action.payload,
      name: product.title,
      price: product.price,
      quantity: 1,
    });
  }

  return {
    ...state,
    items: updatedItems,
  };
};

const updateCartItemQuantity = (state, action) => {
  const updatedItems = [...state.items];
  const updatedItemIndex = updatedItems.findIndex(
    (item) => item.id === action.payload.productId
  );

  const updatedItem = {
    ...updatedItems[updatedItemIndex],
  };

  updatedItem.quantity += action.payload.amount;

  if (updatedItem.quantity <= 0) {
    updatedItems.splice(updatedItemIndex, 1);
  } else {
    updatedItems[updatedItemIndex] = updatedItem;
  }

  return {
    items: updatedItems,
  };
};

export default function ShoppingCartContextProvider({ children }) {
  const [shoppingCartState, shoppingCartDispatch] = useReducer(
    shoppingCartReducer,
    {
      items: [],
    }
  );

  const handleAddItemToCart = (id) => {
    shoppingCartDispatch({
      type: "ADD_ITEM",
      payload: id,
    });
  };

  const handleUpdateCartItemQuantity = (productId, amount) => {
    shoppingCartDispatch({
      type: "UPDATE_ITEM",
      payload: {
        productId,
        amount,
      },
    });
  };

  const result = {
    items: shoppingCartState.items,
    addItemToCart: handleAddItemToCart,
    updateCartItemQuantity: handleUpdateCartItemQuantity,
  };

  return (
    <ShoppingCartContext.Provider value={result}>
      {children}
    </ShoppingCartContext.Provider>
  );
}
