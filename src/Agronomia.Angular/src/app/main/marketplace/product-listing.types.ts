export type ProductCard = {
  title: string;
  description: string;
  imageUrl: string;
  price: string;
  originalPrice?: string;
  badge?: {
    label: string;
    tone: 'primary' | 'danger' | 'info';
    icon?: string;
  };
  vendor: {
    name: string;
    avatar?: string;
    accent?: string;
  };
  poll?: {
    progress: number;
    currentLabel: string;
    targetLabel: string;
    timeLeft?: string;
    highlight?: string;
  };
  status?: 'soldout';
};

export type FilterGroup = {
  title: string;
  options: {
    label: string;
    count?: number;
    selected?: boolean;
  }[];
};
